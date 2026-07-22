using BaseLib.Abstracts;
using Mariner.MarinerCode.Extensions;
using Godot;

namespace Mariner.MarinerCode.Character;

public class MarinerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Mariner.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}