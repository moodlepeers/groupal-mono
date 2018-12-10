package unipotsdam.gf.modules.group.preferences.groupal;

import javax.xml.bind.annotation.XmlAttribute;
import javax.xml.bind.annotation.XmlElement;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class Participant {

    private int id;
    private java.util.List<Criterion> criterion;

    public Participant() {
        criterion = new ArrayList<>();

    }

    @XmlAttribute
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @XmlElement(name = "Criterion")
    public List<Criterion> getCriterion() {
        return criterion;
    }

    public void setCriterion(List<Criterion> criterion) {
        this.criterion = criterion;
    }
}
