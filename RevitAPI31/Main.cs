using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI31
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите стены по грани");
            
            for (int i = 0; i < selectedElementRefList.Count; i++)
            {
                var selectedElement = doc.GetElement(selectedElementRefList.ElementAt(i));
                if (selectedElement is Wall)
                {
                    Parameter volumeParameter = selectedElement.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    if (volumeParameter.StorageType == StorageType.Double)
                    {
                        double volumeValue = UnitUtils.ConvertToInternalUnits(volumeParameter.AsDouble(), UnitTypeId.Meters);
                        TaskDialog.Show("Volume", volumeValue.ToString());
                    }
                }
            }

            // Почему то если использовать foreach, get_Parameter/LookupParameter не видят Reference. Придумал сделать так.

            return Result.Succeeded;
        }
    }
}

