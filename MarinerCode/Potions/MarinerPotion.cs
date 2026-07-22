using BaseLib.Abstracts;
using BaseLib.Utils;
using Mariner.MarinerCode.Character;

namespace Mariner.MarinerCode.Potions;

[Pool(typeof(MarinerPotionPool))]
public abstract class MarinerPotion : CustomPotionModel;