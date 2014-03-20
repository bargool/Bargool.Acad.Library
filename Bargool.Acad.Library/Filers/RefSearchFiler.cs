/*
 * User: aleksey.nakoryakov
 * Date: 22.06.12
 * Time: 11:55
 */
//Microsoft
using System;
using System.Linq;

//Autodesk
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace Bargool.Acad.Library.Filers
{
	/// <summary>
	/// See http://www.theswamp.org/index.php?topic=42034.msg471839#msg471839
	/// </summary>
	public class RefSearchFiler : Autodesk.AutoCAD.DatabaseServices.DwgFiler
	{
		private ObjectId referencedId;
		private DBObject current = null;
		
		private ObjectIdCollection referencingObjectIds = new ObjectIdCollection();
		// Pass the Id of the object to find references to:
		
		public RefSearchFiler( ObjectId referencedId )
		{
			this.referencedId = referencedId;
		}
		
		// Search the parameter for references to the referencedId:
		
		public void Find( DBObject obj )
		{
			this.current = obj;
			try
			{
				obj.DwgOut( this );
			}
			finally
			{
				this.current = null;
			}
		}
		
		// The ObjectIds of all referencing objects:
		
		public ObjectIdCollection ReferencingObjectIds
		{
			get
			{
				return referencingObjectIds;
			}
		}
		
		public override void WriteSoftOwnershipId( ObjectId value )
		{
			OnWriteReference( value );
		}
		
		public override void WriteSoftPointerId( ObjectId value )
		{
			OnWriteReference( value );
		}
		
		public override void WriteHardOwnershipId( ObjectId value )
		{
			OnWriteReference( value );
		}
		
		public override void WriteHardPointerId( ObjectId value )
		{
			OnWriteReference( value );
		}
		
		// In this case, we don't care about whether a reference
		// is hard/soft/owner/pointer, but we could discriminate
		// further if needed.
		
		void OnWriteReference( ObjectId value )
		{
			if( value == referencedId && this.current != null )
			{
				referencingObjectIds.Add( this.current.ObjectId );
				this.current = null;
			}
		}
		// ************************************************************
		
		public override void WriteVector3d(Autodesk.AutoCAD.Geometry.Vector3d value)
		{
		}
		
		public override void WriteVector2d(Autodesk.AutoCAD.Geometry.Vector2d value)
		{
		}
		
		public override void WriteUInt64(ulong value)
		{
		}
		
		public override void WriteUInt32(uint value)
		{
		}
		
		public override void WriteUInt16(ushort value)
		{
		}
		
		public override void WriteString(string value)
		{
		}
		
		public override void WriteScale3d(Autodesk.AutoCAD.Geometry.Scale3d value)
		{
		}
		
		public override void WritePoint3d(Autodesk.AutoCAD.Geometry.Point3d value)
		{
		}
		
		public override void WritePoint2d(Autodesk.AutoCAD.Geometry.Point2d value)
		{
		}
		
		public override void WriteInt64(long value)
		{
		}
		
		public override void WriteInt32(int value)
		{
		}
		
		public override void WriteInt16(short value)
		{
		}
		
		public override void WriteHandle(Autodesk.AutoCAD.DatabaseServices.Handle handle)
		{
		}
		
		public override void WriteDouble(double value)
		{
		}
		
		public override void WriteBytes(byte[] value)
		{
		}
		
		public override void WriteByte(byte value)
		{
		}
		
		public override void WriteBoolean(bool value)
		{
		}
		
		public override void WriteBinaryChunk(byte[] chunk)
		{
		}
		
		public override void WriteAddress(IntPtr value)
		{
		}
		
		public override void Seek(long offset, int method)
		{
		}
		
		public override void ResetFilerStatus()
		{
		}
		
		public override Autodesk.AutoCAD.Geometry.Vector3d ReadVector3d()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.Geometry.Vector2d ReadVector2d()
		{
			throw new NotImplementedException();
		}
		
		public override ulong ReadUInt64()
		{
			throw new NotImplementedException();
		}
		
		public override uint ReadUInt32()
		{
			throw new NotImplementedException();
		}
		
		public override ushort ReadUInt16()
		{
			throw new NotImplementedException();
		}
		
		public override string ReadString()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.ObjectId ReadSoftPointerId()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.ObjectId ReadSoftOwnershipId()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.Geometry.Scale3d ReadScale3d()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.Geometry.Point3d ReadPoint3d()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.Geometry.Point2d ReadPoint2d()
		{
			throw new NotImplementedException();
		}
		
		public override long ReadInt64()
		{
			throw new NotImplementedException();
		}
		
		public override int ReadInt32()
		{
			throw new NotImplementedException();
		}
		
		public override short ReadInt16()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.ObjectId ReadHardPointerId()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.ObjectId ReadHardOwnershipId()
		{
			throw new NotImplementedException();
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.Handle ReadHandle()
		{
			throw new NotImplementedException();
		}
		
		public override double ReadDouble()
		{
			throw new NotImplementedException();
		}
		
		public override void ReadBytes(byte[] value)
		{
			throw new NotImplementedException();
		}
		
		public override byte ReadByte()
		{
			throw new NotImplementedException();
		}
		
		public override bool ReadBoolean()
		{
			throw new NotImplementedException();
		}
		
		public override byte[] ReadBinaryChunk()
		{
			throw new NotImplementedException();
		}
		
		public override IntPtr ReadAddress()
		{
			throw new NotImplementedException();
		}
		
		public override long Position {
			get {
				throw new NotImplementedException();
			}
		}
		
		public override Autodesk.AutoCAD.DatabaseServices.FilerType FilerType {
			get {
				return FilerType.BagFiler;
			}
		}
		
		public override Autodesk.AutoCAD.Runtime.ErrorStatus FilerStatus {
			get {
				return new ErrorStatus();
			}
			set {
				throw new NotImplementedException();
			}
		}
	}
}
