-- Special case to remove repeated instances of Commonwealth worldspace ID to reduce DB file size
UPDATE Map_Markers SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE NPC_Search SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE Scrap_Search SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE Standard_Search SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE Position_Data SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE Space_Info SET spaceFormID = '' WHERE spaceFormID = '0000003C';
UPDATE Region SET spaceFormID = '' WHERE spaceFormID = '0000003C';
