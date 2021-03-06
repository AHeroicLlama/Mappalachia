//Run every Mappalachia script consecutively
unit Mappalachia;

uses
	_m_componentQuantity,
	_m_formID,
	_m_interior,
	_m_junkScrap,
	_m_location,
	_m_worldspace;

	function initialize: Integer;
	begin
		AddMessage('Now running _m_componentQuantity...');
		_m_componentQuantity.initialize();

		AddMessage('Now running _m_junkScrap...');
		_m_junkScrap.initialize();

		AddMessage('Now running _m_location...');
		_m_location.initialize();

		AddMessage('Now running _m_formId...');
		_m_formID.initialize();

		AddMessage('Now running _m_interior...');
		_m_interior.initialize();

		AddMessage('Now running _m_worldspace...');
		_m_worldspace.initialize();

		AddMessage('Mappalachia export finished.');
	end;
end.
