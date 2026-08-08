using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Variables;

public class DredgeVar : DynamicVar
{
    public DredgeVar(decimal baseValue) : base("Dredge", baseValue)
    {
        this.WithTooltip();
    }
    
    public DredgeVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip();
    }
}