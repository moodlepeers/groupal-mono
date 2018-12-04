using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupAL.Matcher
{

    /// <summary>
    /// saves the position within the data Matrix for a faster access
    /// </summary>
    public struct RowIndices
    {
        public int i;
        public int j;
        public int k;
        public int l;
    }

    /// <summary>
    /// Save
    /// </summary>
    class OmadoMatrix
    {
        private List<Participant> entries;

        List<Participant>[,,,] matrix;
        int countOfBins = 0;
        List<RowIndices> allocatedRows = new List<RowIndices>();
        Dictionary<RowIndices, int> positiondList = new Dictionary<RowIndices, int>();
        public RowIndices lastRow { get; set; }

        public OmadoMatrix(List<Participant> entries,int countOfBins)
        {
            this.entries = entries;
            this.countOfBins = countOfBins;
            
            //init matrix
            matrix = new List<Participant>[countOfBins, countOfBins, countOfBins, countOfBins];
            for (int i = 0; i < countOfBins; i++)
            {
                for (int j = 0; j < countOfBins; j++)
                {
                    for (int k = 0; k < countOfBins; k++)
                    {
                        for (int l = 0; l < countOfBins; l++)
                        {
                            matrix[i, j, k, l] = new List<Participant>();
                        }
                    }
                }
            }
            // destribute the entries over the matrix
            destributeEntriesOverTheMatrix(entries);
        }

        /// <summary>
        /// Save all participants in the four dimensional datamatrix. the desribution depends on how many bins are assigned to countOfBins
        /// </summary>
        /// <param name="entries"></param>
        private void destributeEntriesOverTheMatrix(List<Participant> entries)
        {
            
            foreach (Participant p in entries) {
                AddToMatrix(p);
            }
        }

        /// <summary>
        /// adds a participant to the  foru dimensional dataMatrix
        /// </summary>
        /// <param name="p"></param>
        private void AddToMatrix(Participant p)
        {
            int i = mapFromContinousScaleToDiscret(p.Criteria[0].Value[0], 0, 1);
            int j = mapFromContinousScaleToDiscret(p.Criteria[0].Value[1], 0, 1);
            int k = mapFromContinousScaleToDiscret(p.Criteria[0].Value[2], 0, 1);
            int l = mapFromContinousScaleToDiscret(p.Criteria[0].Value[3], 0, 1);

            matrix[i, j, k, l].Add(p); 
        }

        /// <summary>
        /// Maps the incoming continious value (0,1) to one of the countOfBins discrete values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>maped Value which is the position in the dataMatrix</returns>
        int mapFromContinousScaleToDiscret(float value, float min, float max) {
            int bin = 0;
            float delta = (max - min) / countOfBins;
            for (int i = 0; i < countOfBins; i++) 
            {
                if (value >= min + i * delta && value < min + (i+1) * delta) 
                {
                    bin = i;
                    return bin;
                }
            }
            return bin;
        }

        /// <summary>
        /// deallocate the allocated rows of the matrix
        /// </summary>
        internal void freeMatrix()
        {
            allocatedRows.Clear();
        }

        /// <summary>
        /// takes one participant out of the matrix, avoid the entries in allocated rows.
        /// </summary>
        /// <returns></returns>
        internal Participant NextParticipant()
        {
            positiondList.Clear();
                // get the person from a cell with most participants
                for (int i = 0; i < countOfBins; i++)
                {
                    for (int j = 0; j < countOfBins; j++)
                    {
                        for (int k = 0; k < countOfBins; k++)
                        {
                            for (int l = 0; l < countOfBins; l++)
                            {
                                RowIndices row;
                                row.i = i;
                                row.j = j;
                                row.k = k;
                                row.l = l;
                                //if row is not allocated
                                if (!rowIsAllocated(row))
                                {
                                    //if there is at least a participant in this cell add the coordinates and the count of participants aof the cell to positionList
                                    if (matrix[i, j, k, l].Count > 0)
                                    {
                                        positiondList.Add(row, matrix[i, j, k, l].Count);
                                        
                                    }
                                }
                            }
                        }
                    }
                }
                if (positiondList.Count == 0) return null;
                int MaxValue = positiondList.Max(x => x.Value);
                var orderd = positiondList.OrderByDescending(x => x.Value);
                IEnumerable<KeyValuePair<RowIndices, int>> candidates = orderd.TakeWhile(x => x.Value == MaxValue);
            
            return getParticipant(candidates);
            
            //IEnumerable<List<Participant>> listmatrix = matrix.Cast<List<Participant>>();
            //List<Participant> cell = listmatrix.Max();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidates">all cells with the same count of participants</param>
        /// <returns>Returns the optimal Participant</returns>
        private Participant getParticipant(IEnumerable<KeyValuePair<RowIndices, int>> candidates)
        {
            RowIndices row=new RowIndices();
            if (candidates.Count() == 0)
            {
                throw new Exception("OmadoGenesis Matcher: 0 candidates shouldn't happen");
            }
            if (candidates.Count() == 1)
            {
                row= candidates.ElementAt(0).Key;
            }
            if (candidates.Count() > 1) 
            {
                row = getNearestRowToLast(lastRow,candidates);
            }

            lastRow = row;
            allocatedRows.Add(lastRow);
            Participant p = matrix[row.i, row.j, row.k, row.l].First();
            matrix[row.i, row.j, row.k, row.l].Remove(p);
            return p;
        }

        private RowIndices getNearestRowToLast(RowIndices lastRow, IEnumerable<KeyValuePair<RowIndices, int>> candidates)
        {
            float minDist=float.PositiveInfinity;
            
            RowIndices nearestRow=new RowIndices();
            foreach (KeyValuePair<RowIndices, int> cand in candidates) 
            {
                float tmpMinDist = euklidDist(lastRow, cand);
                if (tmpMinDist < minDist) 
                {
                    minDist = tmpMinDist;
                    nearestRow = cand.Key;
                }
 
            }
            return nearestRow;
        }

        private float euklidDist(RowIndices lastRow, KeyValuePair<RowIndices, int> cand)
        {
            double distance = 0;
            distance += Math.Pow((lastRow.i - cand.Key.i), 2);
            distance += Math.Pow((lastRow.j - cand.Key.j), 2);
            distance += Math.Pow((lastRow.k - cand.Key.k), 2);
            distance += Math.Pow((lastRow.l - cand.Key.l), 2);

            distance = Math.Sqrt(distance);
            return (float)distance;
        }

        private bool rowIsAllocated(RowIndices row)
        {
            return allocatedRows.Contains(row);
        }

        
    }
}
