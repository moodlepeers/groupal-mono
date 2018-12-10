package unipotsdam.gf.modules.group.preferences.groupal;

import javax.xml.bind.annotation.XmlElement;
import java.util.ArrayList;
import java.util.List;

public class UsedCriteria {
    private java.util.List<UsedCriterion> criterions;

    public UsedCriteria(List<UsedCriterion> criterions) {
        this.criterions = criterions;
    }

    public UsedCriteria() {
        criterions = new ArrayList<>();
    }

    @XmlElement(name = "Criterion")
    public List<UsedCriterion> getCriterions() {
        return criterions;
    }

    public void setCriterions(List<UsedCriterion> criterions) {
        this.criterions = criterions;
    }
}
