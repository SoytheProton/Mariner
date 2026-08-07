using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Mariner.MarinerCode.Cards;

public static class MarinerCardKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Trawl;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Ballast;
}