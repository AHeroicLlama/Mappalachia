# Advanced Mappalachia Mapping functions

## Legend Group
Under the 'items to plot' list, you will find the Legend Group column. This column is editable and allows you to enter a number.<br/>
By changing the number assigned to each item, you are able to group items together and have them plotted against the same icon.<br/>
For example, by default you may have legend groups 0,1,2 and these are represented by Red, Blue and Yellow icons on the map. If you consider item 1 and 2 to be effectively the same thing, you may want to represent this on the map by plotting them with the same icon. Therefore if you instead edit the legend group to be 0,0,2 - you will see the first two items are plotted with the same icon, but the third item retains its original icon.<br/>
When adding items to the legend list, each item will be assigned a new legend group. However, if you add multiple items at once, they will be grouped under the same legend group.<br/>
You can enter any positive integer into this column.<br/>
The color and shape of the plot icon is defined by the Legend Group, and also the Color Palette and Shape Palette defined in the plot icon settings. (See 'Plot Icon settings' below for more info).<br/>
When incrementing through legend group values, Mappalachia will first use up every color, and will then move up to the next available shape and begin iterating through the available colors again. When it has used every available color and shape combination, it will repeat from the beginning.

## Heatmap Mode
Mappalachia has two plotting modes - these are icon mode (on by default) and heatmap mode. You can toggle these modes by navigating to Plot Settings > Plot Mode.<br/>
When in Heatmap mode, Mappalachia will represent the density of items with an intensity of color on the map.<br/>
Heatmap mode is more appropriate for large collections of items such as flora - where it is more valuable to know not the exact positions of items, but rather where they have the highest density, and therefore where you should go to find the most.<br/>

### Weighting
Heatmap mode is uniquely powerful because it allows us to display a range of brightnesses or 'weight' per item. We can leverage this to assign a weight to mapped items.<br/>
Therefore, you will find that when mapping Scrap or NPCs in heatmap mode, each item will not have equal weight, but instead the brightness they generate on the heatmap will represent the amount of scrap they give, or the spawn chance of the NPC.<br/>
For example, standard search items will all have a weight of 1.0, but an NPC  with a 50% spawn chance will have just a 0.5 weight, also for example, a junk item containing 1 lead will have a weight of 1.0, but a junk item containing 3 lead will have a weight of 3.0.<br/>
For this reason, heatmap mode is vastly superior when mapping junk, as standard icon mode cannot imply the value or 'weight' of items (only if they exist or not).<br/>
It is also great for NPC mapping as it doesn't just map the *potential* spawn amounts but actually the *expected* spawn amounts.

### Heatmap Resolution
You can adjust the resolution of the heatmap by navigating to Plot Settings > Heatmap Settings > Resolution.<br/>
The resolution indicates the number of grid squares which comprise the heatmap and hence the precision. You may want to play with the resolution to get the best from your map. Too high a resolution can result in the map just being a series of separate dots, and too low of a resolution can reduce the precision of your map and hide any bright spots.

### Heatmap Color Mode
You can change the 'Color Mode' of the heatmap by navigating to Plot Settings > Heatmap Settings > Color Mode.<br/>
Here you can select either Mono or Duo;<br/>
In Mono mode, every item added to the map is plotted against the same red color. You can't differentiate items from each other but if you want to find the area with the most of *everything* on your map, or are just mapping a single type of item, this is the ideal mode.<br/>
In Duo mode, items are mapped in either red or blue, and areas with an equal amount of both would therefore gain a purple color. This mode is great for differentiating the varying densities of two *different* collections of items, or if you need to guarantee the presence of *both* collections in a given location.<br/>
In Duo Mode, the color assigned is defined by the number in the 'Legend Group' column of the legend items list. You can edit this value to group items together - even numbers will be red, and odd numbers will be blue.

## Draw Volumes
Draw volumes mode is located at Plot Settings > Draw Volumes, and is on by default.<br/>
This setting tells Mappalachia that if it can (EG If it is in Icon mode, and the volume is large enough) - that in-game volumes and triggers (these being defined volumes of space - often invisible in-game). Should have their boundaries drawn instead of just being plotted as a single icon.<br/>
This is probably best understood by trying it out yourself - try searching for NoCampAllowedTrigger with the 'Activator' filter enabled. With 'Draw Volumes' enabled, you will not only see *where* you can't build CAMPs, but also the precise extents of the areas where you cannot build.<br/>
Volumes below a certain area will still be drawn as an icon, in order for them to be easily visualized.

## Plot Icon settings
By navigating to Plot Settings > Plot Icon Settings you can access full control over the visuals of the icons displayed on the map.<br/>
These settings only apply when the 'Plot mode' is 'Icon'.<br/>
The first four settings control the size (total diameter) of the icons, their width (thickness of the lines which make them up), their opacity (or lack of transparency), and finally the darkness of the drop shadow under each icon.<br/>

### Colors and shapes
At the bottom of the form are two collections or 'Palettes' - of colors and shapes.<br/>
These palettes are used to determine the available combinations of shapes and colours that will be used to generate icons to plot items on the map. Inherently, by adding new shapes and colors you create a greater number of possible icons for Mappalachia to use, and by removing them you reduce the available icons.<br/>
On the left hand side is the color Palette. You may add or remove colors to the palette, or select a premade palette using the drop-down. You can bulk-remove colors by selecting multiple before pressing remove.<br/>
On the right hand side is the shape palette. You can cycle through the settings for each shape by selecting each shape in the list view. Adjust the settings of each shape by toggling the shape checkboxes while it is selected. If you add a new shape it will be created with random settings, which you can later edit. You may also remove the selected shape by pressing Remove Selected<br/>

You can reset all Plot Icon Settings by pressing Reset to Default in the bottom left.
