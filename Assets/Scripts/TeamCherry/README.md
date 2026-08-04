# TeamCherry assemblies — decompiled sources

This directory contains C# source reconstructed from the managed assemblies in
`Assets/Plugins`. The original DLLs were not modified.

## Output inventory

| Assembly | C# files |
| --- | ---: |
| `TeamCherry.BuildBot.dll` | 4 |
| `TeamCherry.Cinematics.dll` | 10 |
| `TeamCherry.Localization.dll` | 14 |
| `TeamCherry.NestedFadeGroup.dll` | 25 |
| `TeamCherry.SharedUtils.dll` | 29 |
| `TeamCherry.Splines.dll` | 14 |
| `TeamCherry.TK2D.dll` | 70 |
| **Total** | **166** |

Each assembly has its own subdirectory and generated `.csproj` file. Namespaces
are represented as nested directories.

## Method

- Decompiler: ILSpy / `ilspycmd` 9.1.0.7988
- Output mode: project, one source file per type
- Reference lookup path: `Assets/Plugins`
- Source assembly hashes: see `SHA256SUMS.txt`

## Notes

Decompiled source is a semantic reconstruction, not the original source tree.
Comments, original formatting, some local-variable names, project settings, and
conditional-compilation context cannot be recovered from DLL metadata. The
generated projects also require the matching Unity managed assemblies and the
plugin dependencies from `Assets/Plugins` before they can be rebuilt outside
Unity.
