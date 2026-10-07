using Terraria.UI;

namespace ThoriumAccessoryExpansion.UI.DarkMatter;

public class DarkMatterKineticUI : UIState
{
    public DarkMatterKineticBar KineticBar { get; private set; } = null!;

    public override void OnInitialize()
    {
        KineticBar = new DarkMatterKineticBar();

        Append(KineticBar);
    }
}