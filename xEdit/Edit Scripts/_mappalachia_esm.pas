// Gets indexed ESM file list
// Header 'Index,fileName'
unit _mappalachia_esm;

    uses _mappalachia_lib;

    var outputStrings : TStringList;

    function Initialize: Integer;
    var
        targetESM : IInterface;
        i : Integer;
        outputFile, fileName : string;
    begin
        AddMessage('Exporting ESM data');
        outputFile := ProgramPath + 'Output\ESM.csv';
        outputStrings := TStringList.Create;

        for i := 0 to FileCount() - 1 do begin
            targetESM := FileByIndex(i);
            fileName := GetFileName(targetESM);

            if (GetIsEsm(targetESM) = true) then begin
                outputStrings.Add(
                    IntToStr(i) + ',' +
                    fileName
                );
            end;
        end;

        outputToFile(i, outputFile, outputStrings);
    end;
end.
