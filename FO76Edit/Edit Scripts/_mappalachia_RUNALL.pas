// Run every Mappalachia script consecutively
unit Mappalachia;

uses
	_mappalachia_component,
	_mappalachia_entity,
	_mappalachia_scrap,
	_mappalachia_location,
	_mappalachia_locationCell,
	_mappalachia_position,
	_mappalachia_region,
	_mappalachia_space;

	function initialize: Integer;
	begin
		_mappalachia_space.initialize();
		_mappalachia_location.initialize();
		_mappalachia_locationCell.initialize();
		_mappalachia_region.initialize();
		_mappalachia_scrap.initialize();
		_mappalachia_component.initialize();
		_mappalachia_entity.initialize();
		_mappalachia_position.initialize();

		AddMessage('Full Mappalachia export finished.');
	end;
end.
