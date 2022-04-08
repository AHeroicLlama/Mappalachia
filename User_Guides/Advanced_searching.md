# Advanced Mappalachia Search functions

## Search filters
Under the 'Standard Search' tab, you will have noticed two filters - 'Category' and 'Lock Level'.<br/>
By removing check marks from the boxes here, you can exclude certain types of items from your search. For example, searching "cat" with no filters can yield many different results, but if you press 'Deselect all' from the 'Category' group, and then re-check 'NPC' you will find a much narrower results set.<br/>
You can also, for example only check 'Level 3' under 'Lock Level' and search 'Safe' to find just safes which have a level 3 lock.<br/>
Combining these different filters can be very powerful.<br/>

Note: Due to oddities in how Fallout 76 is built, a great number of items are categorized in-game as 'Loot' (notably, most natural resources). Due to this, it is generally recommended to leave the 'Loot' filter checked if you're uncertain.

## Search field
The search field operates as a case-insensitive 'contains' search - meaning anything with your search term anywhere within it will be returned, for example "cap" will return results for "FloraFirecap".<br/>
This searches both the in-game displayed name (where applicable), and Bethesda's internal name for the object.<br/>
Additionally the space character is treated as a wildcard for any number of characters, meaning "Grafton Monster" will be able to return matches for "GraftonMonster".<br/>
Leaving the search field blank will return everything (while still respecting selected filters).<br/>
Finally, data miners and modders will find that you are able to search for items via their FormID too. You can view the FormID for returned items by selecting Search Settings > Show FormID.<br/>

## Searching in all Spaces
By navigating to Search Settings and toggling on 'Search in all Spaces' you can see search results from every accessible location in-game, both surface world and indoors.<br/>
You cannot however plot items which belong to a different space than the one currently selected. To learn more about making maps for other spaces, please see [**Interiors and other Spaces**](Choosing_spaces.md).

## NPC Search
By changing the tab at the top to 'NPC or Scrap Search' you may use a separate search which can intelligently indicate NPC spawns, most notably including the many cases where NPC Spawns are not guaranteed but instead selected from a pool of options.<br/>
You will notice that there is no search field here but instead you must select the NPC by name. This list is generated dynamically from the data exported from the game.<br/>

You may also define your minimum desired spawn chance. This value will filter search results to only those with a spawn chance greater than or equal to your provided odds.<br/>
This intelligent NPC Search will also aggregate the search results with a 'Standard Search' for the same name of the NPC category.<br/>
For example, by selecting 'Snallygaster' and searching, you should see the top result on the results list will be the variable spawns where Snallygasters *may* spawn (alongside an indicated spawn chance). Then the rest of the results will be other matches for Snallygaster - these being non-variable, guaranteed spawns.<br/>

*Note: The NPC Search is not perfect. You may notice some oddities in this list, for example that there is no Wendigo option, or that most robots are grouped into a generic 'Robot' category. This is again due to the way in which Bethesda built the game - Mappalachia uses this data to produce an almost complete list, but there are numerous complex and niche possible variables which trigger spawning, which might cause the NPC data might be missing a few spawns. However, there should be no false entries, only missing entries, and if an NPC is not present on this list, you may still be able to find them from a 'Standard Search'.*

## Scrap Search
Again, by selecting the 'NPC or Scrap Search' tab you can also search for junk items which contain a given scrap material.<br/>
This feature does not search directly for *junk* but specifically the *scrap* contents of the junk, once they're broken down. The search accounts for both the contents of the junk, and crucially the *amount* of scrap that can be obtained.<br/>
The scrap amount is best visualized in Heatmap mode. See [Advanced Plotting](Advanced_plotting.md) for info on Heatmap mode.<br/>
This function is very simple - simply select your desired scrap and hit search.<br/>
Much like NPC Spawns, the list of available junk is generated dynamically from the in-game data, and any scrap not on the list would indicate that it does not exist naturally in the game world.
