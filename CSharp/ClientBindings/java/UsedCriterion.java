package unipotsdam.gf.modules.group.preferences.groupal;

import javax.xml.bind.annotation.XmlAttribute;

public class UsedCriterion extends Criterion {
    private int valueCount;

    @XmlAttribute
    public int getValueCount() {
        return valueCount;
    }

    public void setValueCount(int valueCount) {
        this.valueCount = valueCount;
    }
}
