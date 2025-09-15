using TMPro;
public class StringPopUp : GenericPopup
{
    public int Id {
        set
        {
            text.text = DataTableManager.stringTable.Get(value);
        }
    }
    public TextMeshProUGUI text;
    public override void Open()
    {
        base.Open();
    }
}