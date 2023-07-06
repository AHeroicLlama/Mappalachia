# Mappalachia Plotting functions

## Plotting Modes
Mappalachia has four plotting modes - these are Icon mode (on by default), Heatmap mode, Topography mode, and Cluster mode. You can toggle these modes by navigating to Plot Settings > Plot Mode.<br/>

## Icon Mode
Icon mode is the default plotting function, and selected items will be mapped using a unique set of pre-defined shapes and colors. It is the most customizable option.

## Topography Mode
Topography mode is similar to Icon mode, except it uses varying colors to visualize the altitude of plotted items. You can select from 2-5 of the first colors defined in the color palette to distinguish the altitude of plotted items. A scale on the right will show which color represents which altitude band (see [Customization](Customization.md) or 'Topography Color Bands' below for more). Due to using varying colors, Topographic plot mode uses icon shapes alone to distinguish between different legend items.<br/>
For items with bounds (volumes), their highest edge is used to determine their height.

### Topography Color Bands
Under Plot Settings > Topography Color Bands you can select the number of unique colors chosen as the altitude key from the color palette. You may select from 2-5 colors and it is generally recommend to use more colors for more dense maps and vice-versa.<br/>
If your color palette contains less colors than the selected value, then only the colors present will be used.

## Heatmap Mode
When in Heatmap mode, Mappalachia will represent the density of items with an intensity of color on the map.<br/>
Heatmap mode is more appropriate for large collections of items such as flora - where it is more valuable to know not the exact positions of items, but rather where they have the highest density, and therefore where you should go to find the most.<br/>

### Weighting
Heatmap mode is uniquely powerful because it allows us to display a range of brightnesses or 'weight' per item. We can leverage this to assign a weight to mapped items.<br/>
Therefore, you will find that when mapping Scrap or NPCs in Heatmap mode, each item will not have equal weight, but instead the brightness they generate on the heatmap will represent the amount of scrap they give, or the spawn chance of the NPC.<br/>
For example, standard search items will all have a weight of 1.0, but an NPC with a 50% spawn chance will have just a 0.5 weight, also for example, a junk item containing 1 lead will have a weight of 1.0, but a junk item containing 3 lead will have a weight of 3.0.<br/>
For this reason, Heatmap mode is vastly superior when mapping junk, as standard Icon mode cannot imply the value or 'weight' of items, but only if they exist or not.<br/>
It is also great for NPC mapping as it doesn't just map the *potential* spawn amounts but actually the *expected* spawn amounts.

### Heatmap Color Mode
You can change the 'Color Mode' of the heatmap by navigating to Plot Settings > Heatmap Settings > Color Mode.<br/>
Here you can select either Mono or Duo;<br/>
In Mono mode, every item added to the map is plotted against the same color (the first color in the color palette). You can't differentiate items from each other but if you want to find the area with the most of *everything* on your map, or are just mapping a single type of item, this is the ideal mode.<br/>
In Duo mode, items are mapped in either of the first two colors in the color palette, and areas with an equal amount of both would therefore be a blend of the two colors. This mode is great for differentiating the varying densities of two *different* collections of items, or if you need to guarantee the presence of *both* collections in a given location.<br/>
In Duo Mode, the color assigned from the palette is defined by the number in the 'Legend Group' column of the legend items list. Even numbers use the first color in the palette, and odd numbers the second.

### Heatmap Resolution
You can adjust the resolution of the heatmap by navigating to Plot Settings > Heatmap Settings > Resolution.<br/>
The resolution indicates the number of grid squares which comprise the heatmap and hence the precision. You may want to play with the resolution to get the best from your map. Too high a resolution can result in the map just being a series of separate dots, and too low of a resolution can reduce the precision of your map and hide any bright spots.

## Cluster mode
Cluster mode groups nearby points into bounded areas or 'clusters' and labels them with the number of entities inside the cluster. It provides additional control and precision versus heatmap mode.<br/>
In cluster mode all items in the legend are plotted together, and there is no distinction between different items. The color used for the clusters is the first color in the palette.<br/>
Cluster settings can be controlled under Plot Settings > Cluster Settings... Here you can fine tune the maximum size and minimum weight desired for exactly your use case.<br/>
The value drawn against the cluster will usually represent the count of items inside it, but for entities with varying spawn chances, it will instead represent the expected count of items given their spawn odds.

### Cluster range
In Cluster Settings, adjust the top slider to set the maximum size of a cluster. It is given that smaller clusters offer more precision.<br/>

### Min Cluster weight
Again in Cluster Settings, you may use the other slider to adjust the minimum cluster weight. (The weight being the expected count of entities within the cluster). Clusters with a weight lower than the minimum are hidden. This feature makes it easier to identify and narrow down on just the highest value clusters.

### Cluster web
By checking 'Cluster Web' in Cluster Settings, you may draw a "web" on the cluster. The web is used to provide further information about the points contained in the cluster. It is formed by drawing a line from every point to the origin of it's cluster.<br/>
The color used for the lines of the web is the second color in the color palette.<br/>

### Live Update
Tick the 'Live Update' checkbox to cause Mappalachia to update the preview whenever you adjust any cluster values.<br/>

## Draw Volumes
Draw volumes mode is located at Plot Settings > Draw Volumes, and is on by default.<br/>
This setting tells Mappalachia to draw the full outline of in-game volumes and triggers instead of just being plotted as a single icon.<br/>
For example, by plotting 'NoCampAllowedTrigger' with 'Draw Volumes' enabled, you will see more precisely *where* you can't build CAMPs, including the precise extents of these areas.<br/>
Very small volumes will be slightly exaggerated to ensure they remain visible. Additionally for performance reasons, some very large volumes will just be represented by an icon.<br/>
Draw Volumes applies to Icon and Topography mode.

## Fill Regions
This option defines if regions plotted with the region search are drawn with a transparent fill, or are left as just their edges.

## Show Reference FormIDs
An advanced option for data miners, selecting Plot Settings > Show Reference Form IDs will annotate the Form ID of plotted entities onto the map, below their plot.<br/>
This only applies to Icon or Topographic plots of entities from the 'Standard' search.

## Legend Group
Under the 'items to plot' list, you will find the Legend Group column. This column is editable and allows you to enter a number.<br/>
By changing the number assigned to each item, you are able to group items together and have them plotted against the same icon.<br/>
For example, by default you may have legend groups 0,1,2 and these are represented by Red, Blue and Yellow icons on the map. If you consider item 1 and 2 to be effectively the same thing, you may want to represent this on the map by plotting them with the same icon. Therefore if you instead edit the legend group to be 0,0,2 - you will see the first two items are plotted with the same icon, but the third item retains its original icon.<br/>
You can enter any positive integer into this column.<br/>
When adding items to the legend list, each item will be assigned a new legend group. However, if you select 'Add as Group', they will be added under the same legend group.<br/>
The color and shape of the plot icon is defined by the Legend Group, and also the Color Palette and Shape Palette defined in the plot style settings. (See [Customization](Customization.md) for more on style settings).<br/>
When plotting unique legend groups, Mappalachia will use every available color and shape combination before repeating icons.

### Overriding and grouping the legend
By editing the 'Display name' column of the legend list, you are able to override and hence group the legend text for given legend groups. Simply enter your desired legend against the legend group you wish to overwrite.<br/>
By doing this, not only do items of the same group share the same color and icon - they also fall under just a single entry on the legend. This is designed so that you can make maps of many generic things (EG, all Flora) while keeping the legend concise.<br/>
You can reset an item back to its default legend by deleting the text in the Display name field.<br/>
If you adjust a legend group of an already overridden legend to that of another overridden legend, all items will inherit the original overriding name of the new group.