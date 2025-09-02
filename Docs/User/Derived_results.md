# Derived results

Some search results are considered "derived" (Most NPCs, some Scrap, and Flux).<br/>
These are entities which are not *directly* spawned in the world, but Mappalachia is able to imply that they effectively exist where they are plotted.<br/>

* NPCs: Derived NPCs have a varying spawn chance, and represent a spawner which may spawn a variety of NPCs from a predefined pool.
* Scrap: Derived Scrap represents the scrap contents of a junk item, which it yields when scrapped.
* Flux: Derived Flux represents where flux can be found on a plant, given they are within a nuke zone.

These items can be found in the same way as any other, via the search feature.<br/>

## Weighted chance
Derived NPCs and Derived Scrap are the main items which make use of the 'Weighted Chance' field on the search results table.<br/>
For NPCs, the weighted chance represents the chance of the NPC spawning at the given location, and therefore is 100% or less.<br/>
For Scrap, the weighted chance represents the quantity of the named scrap in the plotted junk item, and therefore is 100% or over. (For example 300% means 3 of the named scrap.)

## Next guide
[Space selection and misc advanced features](Advanced.md)
