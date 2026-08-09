using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Variables;

public class SubmergeVar : DynamicVar
{
    public SubmergeVar(decimal baseValue) : base("Submerge", baseValue)
    {
        this.WithTooltip("MARINER-SUBMERGE_DYNAMIC");
    }
    
    public SubmergeVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip("MARINER-SUBMERGE");
    }
}