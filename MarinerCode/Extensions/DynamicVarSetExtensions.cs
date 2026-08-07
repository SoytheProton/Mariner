using Mariner.MarinerCode.Cards.Variables;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Extensions;

public static class DynamicVarSetExtensions
{
    public static SubmergeVar Submerge(this DynamicVarSet vard)
    {
        return (SubmergeVar)vard._vars[nameof(Submerge)];
    }
    
    public static DredgeVar Dredge(this DynamicVarSet vard)
    {
        return (DredgeVar)vard._vars[nameof(Dredge)];
    }
}