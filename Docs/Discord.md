# Discord Chat logs

## almightynassar — Today at 18:12

Hey everyone, just starting out and I can't find the answer to a particular question.....

Trying to change an AbilityDef (Convert's convertPowerFactor in this case), and placed the code under StaticConstructorOnStartup so that it is executed after RimWorld has loaded. Issue is that the DefDatabase hasn't loaded AbilityDefs at this stage, so it cannot find the Convert AbilityDef at startup. The code works fine when you edit it in my Mod Setting since by then the AbilityDef is now loaded (tested changing the factor on both the title screen and in a running game. The tool-tip for convert reflects the new power factor value).

What am I missing? I thought Defs are meant to be loaded by the time StaticConstructorOnStartup boots up my code, but it's giving me start-up errors that the Convert AbilityDef isn't loaded (in fact no AbilityDefs are loaded). I had a look at hooks but they don't really meet my needs; I just want to load up my new custom value on start up.

My code on github (from line 79 onwards):

https://github.com/almightynassar/rimworld-AlmightyOlivePatches/blob/main/Source/AlmightyOlivePatches/Settings.cs

## Nabber — Today at 18:16

Refer to the pins to see the order the game actually loads stuff in, the static constructors are in fact loaded quite late, but if you are working with settings you are actually working at Mod subclass creation time

When working with settings I always use the DefName and then have a property that resolves the string to its corresponding Def

Like this. The string is scribed and persisted and the runtime code simply uses the property

```
public override string Label => $"{ButtonTranslationKey.Translate()}: {TargetRace.label}";
string targetRaceDefName;
ThingDef TargetRace => targetRaceDefName == null ? null ? ThingDef.Named(targetRaceDefName);
```

## legodude17, Almighty Verb-Lord — 01/04/2022
@Nabber Bit late, but here's RimWorld's whole loading process in order:
1. Build mod list
2. Load mod load folders
3. Load all mod assemblies, in load order
4. Construct Mod subclasses
5. Load and parse Def xml files
6. Combine all the Def files into 1 large document
7. Load translation keys from Defs (very rarely used)
8. Load and error check patches
9. Apply patches
10. Register Inheritance (Name and ParentName)
11. Apply inheritance
12. Load Defs from XML into Def classes (maintains a list of cross-references)
13. Warn if any patches failed
14. Load Language metadata
15. Put Defs into DefDatabases
16. Put Defs into DefOf fields
17. Build translation key mappings
18. Inject DefInjected translations, the first time
19. Generate implied defs, before resolve (Blueprints, Meat, Frames, Techprints, Corpses, Stone terrain, Recipes from recipeMakers, Neurotrainers)
20. Resolve cross references (goes through the earlier list and actually gets the Def objects and assigns them to the proper fields)
21. Put Defs into DefOf fields, again (this time it will error if it can't find the Def)
22. Reload player knowledge database (used in learning helper)
23. Reset all static data
24. Resolve references (calls ResolveReferences on all Defs), specifically in order: ThingCategoryDefs, RecipeDefs, all other defs, ThingDefs
25. Generate implied defs, after resolve (Key bindings)
26. Reset more static data, and make sure smoothing is set up properly
27. If in Dev mode, log all config errors
28. Load keybindings
29. Give short hashes
30. Load audio files from mods
31. Load textures from mods
32. Load strings from mods
33. Load asset bundles from mods
34. Load backstories
35. Inject DefInjected translations, including in backstories, and error if there's a translation for a Def that isn't present
36. Call all StaticConstructorOnStartups
37. Bake static atlases (building damage, and minified overlays (the crate and bag))
38. Force garbage collection