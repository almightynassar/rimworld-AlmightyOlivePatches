#  RimWorld's loading process

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