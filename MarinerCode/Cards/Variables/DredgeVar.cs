using BaseLib.Extensions;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Variables;

public class DredgeVar : DynamicVar
{
    public DredgeVar(decimal baseValue) : base("Dredge", baseValue)
    {
        this.WithTooltip("MARINER-DREDGE_DYNAMIC");
    }
    
    public DredgeVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip("MARINER-DREDGE");
    }
}