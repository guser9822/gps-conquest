using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0]")]
	public partial class TowerEntityControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 12;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _towerNetPosition;
		public event FieldEvent<Vector3> towerNetPositionChanged;
		public InterpolateVector3 towerNetPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 towerNetPosition
		{
			get { return _towerNetPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_towerNetPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_towerNetPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SettowerNetPositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_towerNetPosition(ulong timestep)
		{
			if (towerNetPositionChanged != null) towerNetPositionChanged(_towerNetPosition, timestep);
			if (fieldAltered != null) fieldAltered("towerNetPosition", _towerNetPosition, timestep);
		}
		private Vector2 _towerGPSCoords;
		public event FieldEvent<Vector2> towerGPSCoordsChanged;
		public InterpolateVector2 towerGPSCoordsInterpolation = new InterpolateVector2() { LerpT = 0f, Enabled = false };
		public Vector2 towerGPSCoords
		{
			get { return _towerGPSCoords; }
			set
			{
				// Don't do anything if the value is the same
				if (_towerGPSCoords == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_towerGPSCoords = value;
				hasDirtyFields = true;
			}
		}

		public void SettowerGPSCoordsDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_towerGPSCoords(ulong timestep)
		{
			if (towerGPSCoordsChanged != null) towerGPSCoordsChanged(_towerGPSCoords, timestep);
			if (fieldAltered != null) fieldAltered("towerGPSCoords", _towerGPSCoords, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			towerNetPositionInterpolation.current = towerNetPositionInterpolation.target;
			towerGPSCoordsInterpolation.current = towerGPSCoordsInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _towerNetPosition);
			UnityObjectMapper.Instance.MapBytes(data, _towerGPSCoords);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_towerNetPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			towerNetPositionInterpolation.current = _towerNetPosition;
			towerNetPositionInterpolation.target = _towerNetPosition;
			RunChange_towerNetPosition(timestep);
			_towerGPSCoords = UnityObjectMapper.Instance.Map<Vector2>(payload);
			towerGPSCoordsInterpolation.current = _towerGPSCoords;
			towerGPSCoordsInterpolation.target = _towerGPSCoords;
			RunChange_towerGPSCoords(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _towerNetPosition);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _towerGPSCoords);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (towerNetPositionInterpolation.Enabled)
				{
					towerNetPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					towerNetPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_towerNetPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_towerNetPosition(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (towerGPSCoordsInterpolation.Enabled)
				{
					towerGPSCoordsInterpolation.target = UnityObjectMapper.Instance.Map<Vector2>(data);
					towerGPSCoordsInterpolation.Timestep = timestep;
				}
				else
				{
					_towerGPSCoords = UnityObjectMapper.Instance.Map<Vector2>(data);
					RunChange_towerGPSCoords(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (towerNetPositionInterpolation.Enabled && !towerNetPositionInterpolation.current.UnityNear(towerNetPositionInterpolation.target, 0.0015f))
			{
				_towerNetPosition = (Vector3)towerNetPositionInterpolation.Interpolate();
				//RunChange_towerNetPosition(towerNetPositionInterpolation.Timestep);
			}
			if (towerGPSCoordsInterpolation.Enabled && !towerGPSCoordsInterpolation.current.UnityNear(towerGPSCoordsInterpolation.target, 0.0015f))
			{
				_towerGPSCoords = (Vector2)towerGPSCoordsInterpolation.Interpolate();
				//RunChange_towerGPSCoords(towerGPSCoordsInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TowerEntityControllerNetworkObject() : base() { Initialize(); }
		public TowerEntityControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TowerEntityControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
