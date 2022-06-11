using HGV.Basilius.Client;
using HGV.Stormcrafter;
using Newtonsoft.Json;

var httpClient = new HttpClient();
var metaClient = new MetaClient();
var statsUrl = "https://windrun.io/api/heroes";

var response = await httpClient.GetStringAsync(statsUrl);
var result = JsonConvert.DeserializeObject<Root>(response);
var major = result.Data.Patches.Majors.Last();
var patch = "7.31";
var fallback = "7.30";
var stats = result.Data.Heroes.Select(_ => new { Id = _.Key, Data = _.Value.ContainsKey(patch) ? _.Value[patch] : _.Value[fallback] }).ToList();
var meta = metaClient.GetHeroes().Where(_ => _.AbilityDraftEnabled == true).ToList();

var collection = meta
    .Join(stats, _ => _.Id, _ => _.Id, (lhs, rhs) => new Hero()
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
        Damage = (lhs.AttackDamageMin + lhs.AttackDamageMax) / 2,
        Roles = lhs.Roles,
        WinRate = rhs.Data.WinRate,
        Complexity = lhs.Complexity,
        Picked = false,
    })
    .OrderBy(_ => _.Name)
    .ToList();


var json = JsonConvert.SerializeObject(collection, Formatting.Indented);
await File.WriteAllTextAsync("heroes.json", json);