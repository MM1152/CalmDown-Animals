using TMPro;
public class StringPopUp : GenericPopup
{
    public int Id {
        set
        {
            //FAIL TEXTS ID = 1 , 2 , 3 , 6
            text.text = DataTableManager.stringTable.Get(value);
            if( value == 1 || value == 2 || value == 3 || value == 6)
            {
                SoundManager.Instance.PlayOneShot(SFX.DisAbleSound);
            }
        }
    }
    public TextMeshProUGUI text;
    public override void Open()
    {
        base.Open();
    }
}