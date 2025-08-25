# Miscellaneous and Advanced Searching/Plotting Features

## Filters
At the top of the window you can filter your search results by their category, or their lock type.<br/>
The categories are defined by Bethesda, and therefore sometimes unexpected, so it is typically best to use 'Select Recommended' category filters.

## Space Selection & Searching
Use the 'Selected Space' drop down to change the space for your map. By default search results are only for the selected space. You may select 'Search Settings > 'Search in all Spaces' to show results regardless of space. You can also double-click the space column of the search results to quick-swap to that space.<br/>

If you plot an item which is not in the currently selected space, it will typically not show up. However, if there is a door connecting the two spaces, then these items will be plotted at that doorway, indicating your search results exist in a connecting space.<br/>
Use 'Plot Settings' > 'Auto-find items in other spaces' to automatically find and plot your selected items, wherever they may exist in all connected spaces.<br/>

Additionally, you may filter your search results to only show spaces which are instances ('Search Settings' > 'Search in instances only').

## Volumes and Regions
Items classified as Regions, and a few instances of Activators are represented by a volume. You can choose to show these as filled shapes, or just their border, or both, by selecting 'Plot Settings' > 'Volume Draw Style'.<br/>

Some regions affect the level of NPCs spawned inside them. This level range can be labelled against the Region by selecting 'Plot Settings'. > 'Show Region Levels'

## Show Instance FormIDs
An advanced option for data miners, selecting 'Plot Settings' > 'Show Instance FormID' will annotate the FormID of the plotted entities onto the map against their plot.<br/>
This only applies to Standard or Topographic plot mode.

## Legend Group
When adding items to the map (The 'items selected to plot' list), items will be assigned a unique "legend group" number. If you use the 'Add as group' drop down on the 'Add to Map' button, the items added will share the same legend group.<br/>

The purpose of the legend group is for items to share an icon, and optionally legend text.<br/>
Whenever you edit an icon in the 'items to plot' list, all items of that legend group will be updated also.<br/>
Items sharing a legend group will not by default share the same legend text, however you may edit the legend text directly in the grid; similarly to when icons are changed - if you change the legend text, all other items sharing that group will inherit the same text.<br/>
Finally, you may change the legend group value of an item directly, in order to change its group membership.

## Advanced
A feature for data miners: by toggling 'Search Settings' > 'Advanced Mode' you can enable searching by FormID. You can search the form ID of the instance or its reference. FormIDs can be given with or without the `0x` prefix. This mode will also swap the signature names for their underlying 4-character IDs, and spaces will be named by their EditorID

## Next guide
[Spotlight](Spotlight.md)
