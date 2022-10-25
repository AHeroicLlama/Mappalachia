-- Special case to remove repeated instances of Appalachia worldspace ID to reduce DB file size
UPDATE Map_Markers SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE NPC_Search SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE Scrap_Search SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE Standard_Search SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE Position_Data SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE Space_Info SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
UPDATE Region SET spaceFormID = '' WHERE spaceFormID = '0025DA15';
