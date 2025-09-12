using TMPro;
public class StringPopUp : GenericPopup
{
    public int id;
    public TextMeshProUGUI text;
    public override void Open()
    {
        text.text = DataTableManager.stringTable.Get(id);
        base.Open();
    }
}