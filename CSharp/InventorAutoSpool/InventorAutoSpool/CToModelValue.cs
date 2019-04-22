using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorAutoSpool
{
	public static class CToModelValue
	{

		/// <summary>
		/// Suits Flat Faced Flanges String Expression (e.g 21 mm, 21 ) To Model World Units As Document Default Units 14 Params,
		/// </summary>
		/// <param name="PartDocument">Inventor PartDocument Object As Referance</param>
		/// <param name="szDiaOD">Outside Diameter, Expression As String</param>
		/// <param name="szDiaID">Inside Diameter, Expression As String</param>
		/// <param name="szThickness">Thickness Of The Object, Expression As String</param>
		/// <param name="szPCD">Placed Center Diameter, Expression As String</param>
		/// <param name="szDia">Bolt Hole Diameter, Expression As String</param>
		/// <param name="szNumber">Number Of Bolt Holes In PCD, Expression As String</param>	
		/// <param name="DiaOD">Outside Diameter, Model World Value, Out As double</param>
		/// <param name="DiaID">The Inside Diameter, Model World Value, Out As double</param>
		/// <param name="Thickness">Thickness Of The Object, Model World Value, Out As double</param>
		/// <param name="PCD">Placed Center Diameter, Model World Value, Out As double</param>
		/// <param name="Dia">Bolt Hole Diameter, Model World Value, Out As double</param>
		/// <param name="Number">Number Of Bolt Holes In PCD, Model World Value, Out As double</param>
		/// <returns>returns true if input validated, returns false if one Expression Fails Sets all out Vals to 0.</returns>
		public static bool ConvertFlatFacedFlangeDimsToModelWorldUnits(
			ref Inventor.PartDocument PartDocument, 
			string szDiaOD, 
			string szDiaID, 
			string szThickness, 
			string szPCD,
			string szBoltHoleDia, 
			string szNumber,
			out double DiaOD,
			out double DiaID, 
			out double Thickness,
			out double PCD, 
			out double Dia, 
			out double Number)
		{
			DiaOD = 0;DiaID = 0;Thickness = 0;PCD = 0;Dia = 0;Number = 0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			if(UnitsOfMeasure.IsExpressionValid(szDiaOD, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&& UnitsOfMeasure.IsExpressionValid(szDiaID, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&& UnitsOfMeasure.IsExpressionValid(szThickness, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&& UnitsOfMeasure.IsExpressionValid(szPCD, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&& UnitsOfMeasure.IsExpressionValid(szBoltHoleDia, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&& UnitsOfMeasure.IsExpressionValid(szNumber, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits))
			{
				ConvertBaseObjectDims(ref PartDocument,szDiaOD,szDiaID,szPCD,szThickness,szNumber,
				out DiaOD,out DiaID,out PCD,out Thickness,out Number);
				Dia = UnitsOfMeasure.GetValueFromExpression(szBoltHoleDia, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				Number = UnitsOfMeasure.GetValueFromExpression(szNumber, Inventor.UnitsTypeEnum.kUnitlessUnits);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Suits Flat Faced Flanges String Expression e.g 21 mm, 21, To Model World Units, Set By User.
		/// </summary>
		/// <param name="PartDocument">Inventor PartDocument Object As Referance</param>
		/// <param name="szDiaOD">Outside Diameter, Expression As String</param>
		/// <param name="szDiaID">Inside Diameter, Expression As String</param>
		/// <param name="szThickness">Thickness Of The Object, Expression As String</param>
		/// <param name="szPCD">Placed Center Diameter, Expression As String</param>
		/// <param name="szDia">Bolt Hole Diameter, Expression As String</param>
		/// <param name="szNumber">Number Of Bolt Holes In PCD, Expression As String</param>	
		/// <param name="DiaOD">Outside Diameter, Model World Value, Out As double</param>
		/// <param name="DiaID">The Inside Diameter, Model World Value, Out As double</param>
		/// <param name="Thickness">Thickness Of The Object, Model World Value, Out As double</param>
		/// <param name="PCD">Placed Center Diameter, Model World Value, Out As double</param>
		/// <param name="Dia">Bolt Hole Diameter, Model World Value, Out As double</param>
		/// <param name="Number">Number Of Bolt Holes In PCD, Model World Value, Out As double</param>
		/// <returns>returns true if input validated, returns false if one Expression Fails Sets all out Vals to 0.</returns>
		public static bool ConvertFlatFacedFlangeDimsToModelWorldUnits(
			ref Inventor.PartDocument PartDocument,
			ref Inventor.UnitsTypeEnum UnitsType,
			string szDiaOD,
			string szDiaID,
			string szThickness,
			string szPCD,
			string szBoltHoleDia,
			string szNumber,
			out double DiaOD,
			out double DiaID,
			out double Thickness,
			out double PCD,
			out double Dia,
			out double Number) {
			DiaOD=0; DiaID=0; Thickness=0; PCD=0; Dia=0; Number=0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			UnitsOfMeasure.LengthUnits=UnitsType;
			if(UnitsOfMeasure.IsExpressionValid(szDiaOD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szDiaID,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szThickness,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szPCD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szBoltHoleDia,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szNumber,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)) 
			{
				DiaOD=UnitsOfMeasure.GetValueFromExpression(szDiaOD,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				DiaID=UnitsOfMeasure.GetValueFromExpression(szDiaID,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				Thickness=UnitsOfMeasure.GetValueFromExpression(szThickness,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				PCD=UnitsOfMeasure.GetValueFromExpression(szPCD,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				Dia=UnitsOfMeasure.GetValueFromExpression(szBoltHoleDia,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				Number=UnitsOfMeasure.GetValueFromExpression(szNumber,Inventor.UnitsTypeEnum.kUnitlessUnits);
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// Suits Flat Faced Flanges String Expression (e.g 21 mm, 21 ) To Model World Units As Document Default Units 14 Params,
		/// </summary>
		/// <param name="PartDocument">Inventor PartDocument Object As Referance</param>
		/// <param name="szDiaOD">Outside Diameter, Expression As String</param>
		/// <param name="szDiaID">Inside Diameter, Expression As String</param>
		/// <param name="szThickness">Thickness Of The Object, Expression As String</param>
		/// <param name="szPCD">Placed Center Diameter, Expression As String</param>
		/// <param name="szBoltHoleDia">Bolt Hole Diameter, Expression As String</param>
		/// <param name="szNumber">Number Of Bolt Holes In PCD, Expression As String</param>	
		/// <param name="DiaOD">Outside Diameter, Model World Value, Out As double</param>
		/// <param name="DiaID">The Inside Diameter, Model World Value, Out As double</param>
		/// <param name="Thickness">Thickness Of The Object, Model World Value, Out As double</param>
		/// <param name="PCD">Placed Center Diameter, Model World Value, Out As double</param>
		/// <param name="Dia">Bolt Hole Diameter, Model World Value, Out As double</param>
		/// <param name="Number">Number Of Bolt Holes In PCD, Model World Value, Out As double</param>
		/// <returns>returns true if input validated, returns false if one Expression Fails Sets all out Vals to 0.</returns>
		public static bool ConvertRaisedFacedFlangeDimsToModelWorldUnits(
			ref Inventor.PartDocument PartDocument,
			string szDiaOD,
			string szDiaID,
			string szThickness,
			string szPCD,
			string szBoltHoleDia,
			string szNumber,
			string szRaiseDiaOD,
			string szRaisedDepth,
			out double DiaOD,
			out double DiaID,
			out double Thickness,
			out double PCD,
			out double Dia,
			out double Number,
			out double RaiseDiaOD,
			out double RaisedDepth) {
			DiaOD=0; DiaID=0; Thickness=0; PCD=0; Dia=0; Number=0; RaiseDiaOD=0; RaisedDepth=0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			if(UnitsOfMeasure.IsExpressionValid(szDiaOD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szDiaID,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szThickness,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szPCD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szBoltHoleDia,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szNumber,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szRaiseDiaOD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
				&&UnitsOfMeasure.IsExpressionValid(szRaisedDepth,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)) {
				ConvertBaseObjectDims(ref PartDocument,szDiaOD,szDiaID,szPCD,szThickness,szNumber,
					out DiaOD,out DiaID,out PCD,out Thickness,out Number );

				Dia=UnitsOfMeasure.GetValueFromExpression(szBoltHoleDia,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				RaiseDiaOD=UnitsOfMeasure.GetValueFromExpression(szRaiseDiaOD,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				RaisedDepth=UnitsOfMeasure.GetValueFromExpression(szRaisedDepth,Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// Suits Pipe Converts String Expression (e.g 21 mm, 21 ) To Model World Units As Document Default Units,
		/// </summary>
		/// <param name="PartDocument">Inventor PartDocument Object As Referance</param>
		/// <param name="szDiaOD">Outside Diameter, Expression As String</param>
		/// <param name="szLength">The Length, Expression As String</param>
		/// <param name="szThickness">Thickness Of The Object, Expression As String</param>
		/// <param name="DiaOD">Outside Diameter, Model World Value, Out As double</param>
		/// <param name="Length">The Length, Model World Value, Out As double</param>
		/// <param name="Thickness">Thickness Of The Object, Model World Value, Out As double</param>
		/// <returns>returns true if input validated, returns false if one Expression Fails Sets all out Vals to 0.</returns>
		public static bool ConvertPipeDiamensionsToModelWorldUnits(
			ref Inventor.PartDocument PartDocument, 
			string szDiaOD, 
			string szThickness,
			string szLength, 
			out double DiaOD, 
			out double Thickness,
			out double Length) {
			DiaOD = 0;Length = 0;Thickness = 0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			if(UnitsOfMeasure.IsExpressionValid(szDiaOD, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits) && UnitsOfMeasure.IsExpressionValid(szLength, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits) && UnitsOfMeasure.IsExpressionValid(szThickness, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits))
			{
				DiaOD = UnitsOfMeasure.GetValueFromExpression(szDiaOD, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);	
				Thickness = UnitsOfMeasure.GetValueFromExpression(szThickness, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
				Length=UnitsOfMeasure.GetValueFromExpression(szLength,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Suits Pipe  Converts Expression (e.g 21 mm, 21 )To Model World Units As Set By user,
		/// </summary>
		/// <param name="PartDocument">Inventor PartDocument Object As Referance</param>
		/// <param name="UnitsType">Document Units Type By Inventor Api UnitsTypeEnum</param>
		/// <param name="szDiaOD">Outside Diameter, Expression As String</param>
		/// <param name="szLength">The Inside Diameter, Expression As String</param>
		/// <param name="szThickness">Thickness Of The Object, Expression As String</param>
		/// <param name="DiaOD">Outside Diameter, Model World Value Out As double</param>
		/// <param name="Length">The Inside Diameter, Model World Value Out As double</param>
		/// <param name="Thickness">Thickness Of The Object, Model World Value Out As double</param>
		/// <returns>returns true if input validated, returns false if one Expression Fails Sets all out Vals to 0.</returns>
		public static bool ConvertPipeDiamensionsToModelWorldUnits(
			ref Inventor.PartDocument PartDocument,
			Inventor.UnitsTypeEnum UnitsType,
			string szDiaOD,
			string szThickness,
			string szLength,
			out double DiaOD,
			out double Thickness,
			out double Length) {
			DiaOD=0; Length=0; Thickness=0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			UnitsOfMeasure.LengthUnits=UnitsType;
			if(UnitsOfMeasure.IsExpressionValid(szDiaOD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)&&UnitsOfMeasure.IsExpressionValid(szLength,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)&&UnitsOfMeasure.IsExpressionValid(szThickness,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)) {
				DiaOD=UnitsOfMeasure.GetValueFromExpression(szDiaOD,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
				Length=UnitsOfMeasure.GetValueFromExpression(szLength,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
				Thickness=UnitsOfMeasure.GetValueFromExpression(szThickness,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
				return true;
			}
			else {
				return false;
			}
		}

		private  static void  ConvertBaseObjectDims(
			ref Inventor.PartDocument PartDocument,
			string szWidth,
			string szHeight,
			string szLength,
			string szThickness,
			string szQty,
			out double Width,
			out double Height,
			out double Length,
			out double Thickness,
			out double Qty) {
			Width=0; Height=0; Length=0; Thickness=0;Qty=0;
			Inventor.UnitsOfMeasure UnitsOfMeasure = PartDocument.UnitsOfMeasure;
			Width=UnitsOfMeasure.GetValueFromExpression(szWidth,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
			Height=UnitsOfMeasure.GetValueFromExpression(szHeight,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
			Length=UnitsOfMeasure.GetValueFromExpression(szLength,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
			Thickness=UnitsOfMeasure.GetValueFromExpression(szThickness,Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits);
			Qty=UnitsOfMeasure.GetValueFromExpression(szQty,Inventor.UnitsTypeEnum.kUnitlessUnits );

		}
	}
}

//, out double DiaOD, out double DiaID, out double Thickness