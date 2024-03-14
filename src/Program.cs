using HGV.Basilius.Client;
using HGV.Stormcrafter;
using Newtonsoft.Json;

var metaClient = new MetaClient();

var collection = metaClient.GetHeroes().Where(_ => _.AbilityDraftEnabled == true).Select(lhs => new Hero()
{
  Id = lhs.Id,
  Name = lhs.Name,
  Key = lhs.Key.Replace("npc_dota_hero_", ""),
  Aliases = lhs.NameAliases,
  ImageBanner = lhs.ImageBanner,
  ImageIcon = lhs.ImageIcon,
  ImageProfile = lhs.ImageProfile,
  ImagePortrait = lhs.ImagePortrait,
  AbilityReplaceRequired = lhs.AbilityReplaceRequired,
  StrengthGain = lhs.AttributeStrengthGain,
  MaxStrength = lhs.AttributeBaseStrength + (lhs.AttributeStrengthGain * 30),
  IntelligenceGain = lhs.AttributeIntelligenceGain,
  MaxIntelligence = lhs.AttributeBaseIntelligence + (lhs.AttributeIntelligenceGain * 30),
  AgilityGain = lhs.AttributeAgilityGain,
  MaxAgility = lhs.AttributeBaseAgility + (lhs.AttributeAgilityGain * 30),
  AttributePrimary = lhs.AttributePrimary,
  AttackCapabilities = lhs.AttackCapabilities,
  AttackRange = lhs.AttackRange,
  Damage = (lhs.AttackDamageMin + lhs.AttackDamageMax) / 2,
  Roles = lhs.Roles,
  WinRate = lhs.Winrate,
  Complexity = lhs.Complexity,
  Picked = false,
})
.OrderBy(_ => _.Name)
.ToList();

var json = JsonConvert.SerializeObject(collection, Formatting.Indented);
await File.WriteAllTextAsync("heroes.json", json);