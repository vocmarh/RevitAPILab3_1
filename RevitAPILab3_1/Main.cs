using Autodesk.Revit.ApplicationServices;
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

namespace RevitAPILab3_1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберите элементы");
            var elementList = new List<Wall>();

            double volume = 0;

            foreach (var selectedElement in selectedElementRefList)
            {
                Wall wall = doc.GetElement(selectedElement) as Wall;
                Parameter volumePar = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumePar.StorageType == StorageType.Double)
                {
                    volume += volumePar.AsDouble();
                }                
            }

            double volumeMillimeters = UnitUtils.ConvertFromInternalUnits(volume, DisplayUnitType.DUT_CUBIC_MILLIMETERS);

            TaskDialog.Show("Объем выбранных стен", volumeMillimeters.ToString());

            return Result.Succeeded;
        }
    }
}
