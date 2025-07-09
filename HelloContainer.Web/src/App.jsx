import { useEffect, useState } from 'react';
import './App.css';

function ContainerGraph({ containers, handleConnect, handleDelete, handleAddWater }) {
  const width = 160 * containers.length + 60;
  const height = 220;
  const containerWidth = 60;
  const containerHeight = 100;
  const marginX = 100;
  const marginY = 50;

  const positions = containers.map((c, i) => ({
    id: c.id,
    x: marginX + i * 130,
    y: marginY
  }));

  const getPos = id => positions.find(p => p.id === id);

  const handleDisconnect = async (id1, id2) => {
    if (!window.confirm('Are you sure you want to disconnect these two containers?')) return;
    try {
      const res = await fetch(`/api/containers/disconnections`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ sourceContainerId: id1, targetContainerId: id2 })
      });
      if (!res.ok) throw new Error('Disconnect failed');
      window.location.reload();
    } catch (e) {
      alert(e.message);
    }
  };

  return (
    <svg width={width} height={height} style={{ background: '#f8f8f8', borderRadius: 12, marginBottom: 32 }}>
      {containers.map((c, i) => (
        c.connectedContainerIds?.map(cid => {
          const from = getPos(c.id);
          const to = getPos(cid);
          if (!to || c.id > cid) return null;
          const mx = (from.x + containerWidth + to.x) / 2;
          const my = (from.y + containerHeight / 2 + to.y + containerHeight / 2) / 2;
          return (
            <g key={c.id + '-' + cid}>
              <line
                x1={from.x + containerWidth}
                y1={from.y + containerHeight / 2}
                x2={to.x}
                y2={to.y + containerHeight / 2}
                stroke="#222"
                strokeWidth={2}
              />
              <g
                style={{ cursor: 'pointer' }}
                onClick={() => handleDisconnect(c.id, cid)}
                transform={`translate(${mx - 10}, ${my - 10})`}
              >
                <rect width={20} height={20} rx={6} fill="#fff" stroke="#f44336" strokeWidth={1.5} />
                <line x1={5} y1={5} x2={15} y2={15} stroke="#f44336" strokeWidth={2} />
                <line x1={15} y1={5} x2={5} y2={15} stroke="#f44336" strokeWidth={2} />
              </g>
            </g>
          );
        })
      ))}
      {containers.map((c, i) => {
        const pos = getPos(c.id);
        const waterHeight = containerHeight * (c.amount / c.capacity);
        const isFull = c.amount >= c.capacity;
        const showWater = c.amount > 0;
        return (
          <g key={c.id}>
            <text x={pos.x + containerWidth / 2} y={pos.y - 10} textAnchor="middle" fontSize={14} fill="#666">Capacity: {c.capacity}</text>
            <rect x={pos.x} y={pos.y} width={containerWidth} height={containerHeight} rx={15} fill="#fff" stroke="#222" strokeWidth={2} />
            {showWater && (
              <rect
                x={pos.x}
                y={pos.y + containerHeight - waterHeight}
                width={containerWidth}
                height={waterHeight}
                fill="#90caf9"
                rx={isFull ? 15 : 0}
                ry={isFull ? 15 : 0}
              />
            )}
            <text x={pos.x + containerWidth / 2} y={pos.y + containerHeight / 2 + 6} textAnchor="middle" fontSize={18} fill="#222">{c.amount}</text>
            <text x={pos.x + containerWidth / 2} y={pos.y + containerHeight + 24} textAnchor="middle" fontSize={16}>{c.name}</text>
            {(() => {
              const buttonSize = 24;
              const buttonGap = 12;
              const buttonY = pos.y + containerHeight + 36;
              const buttonStartX = pos.x + containerWidth / 2 - (buttonSize * 1.5 + buttonGap);
              return (
                <g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleAddWater(c.id)}
                    transform={`translate(${buttonStartX}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#bbdefb')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#e3f2fd')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#e3f2fd" />
                    <line x1="12" y1="6" x2="12" y2="18" stroke="#1976d2" strokeWidth="2" />
                    <line x1="6" y1="12" x2="18" y2="12" stroke="#1976d2" strokeWidth="2" />
                  </g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleDelete(c.id)}
                    transform={`translate(${buttonStartX + buttonSize + buttonGap}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#ffcdd2')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#ffebee')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#ffebee" />
                    <rect x="8" y="12" width="8" height="7" rx="2" fill="#f44336" />
                    <rect x="10" y="7" width="4" height="5" rx="1" fill="#f44336" />
                    <line x1="11" y1="15" x2="13" y2="17" stroke="#fff" strokeWidth="1.2" />
                    <line x1="13" y1="15" x2="11" y2="17" stroke="#fff" strokeWidth="1.2" />
                  </g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleConnect && handleConnect(c.id)}
                    transform={`translate(${buttonStartX + (buttonSize + buttonGap) * 2}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#bbdefb')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#e3f2fd')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#e3f2fd" />
                    <circle cx="9" cy="12" r="4" fill="#1976d2" />
                    <circle cx="15" cy="12" r="4" fill="#1976d2" />
                    <line x1="13" y1="12" x2="11" y2="12" stroke="#1976d2" strokeWidth="2" />
                  </g>
                </g>
              );
            })()}
          </g>
        );
      })}
    </svg>
  );
}

function App() {
  const [containers, setContainers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [newContainerCapacity, setNewContainerCapacity] = useState(10);
  const [amountInputs, setAmountInputs] = useState({});
  const [toast, setToast] = useState({ message: '', type: '' }); // type: 'success' | 'error'
  const [newContainerName, setNewContainerName] = useState("");
  const [connectSourceId, setConnectSourceId] = useState(null);
  const [showConnectModal, setShowConnectModal] = useState(false);
  const [addWaterTargetId, setAddWaterTargetId] = useState(null);
  const [addWaterAmount, setAddWaterAmount] = useState(1);

  const fetchContainers = async () => {
    setLoading(true);
    setError('');
    try {
      const res = await fetch('/api/containers');
      if (!res.ok) throw new Error('Failed to fetch containers');
      const data = await res.json();
      setContainers(data);
    } catch (e) {
      setError(e.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchContainers();
  }, []);

  useEffect(() => {
    if (toast.message) {
      const timer = setTimeout(() => setToast({ message: '', type: '' }), 2500);
      return () => clearTimeout(timer);
    }
  }, [toast]);

  const handleAddContainer = async () => {
    setError("");
    if (!newContainerName.trim()) {
      setToast({ message: "Name is required", type: "error" });
      return;
    }
    try {
      const res = await fetch("/api/containers", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name: newContainerName, capacity: newContainerCapacity })
      });
      if (!res.ok) throw new Error("Failed to add container");
      setToast({ message: "Added successfully", type: "success" });
      setNewContainerName("");
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: "error" });
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this container?')) return;
    setError('');
    try {
      const res = await fetch(`/api/containers/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Delete failed');
      setToast({ message: 'Deleted successfully', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  const handleAddWater = (id) => {
    setAddWaterTargetId(id);
    setAddWaterAmount(1);
  };

  const handleConfirmAddWater = async () => {
    setError('');
    try {
      const res = await fetch(`/api/containers/${addWaterTargetId}/water`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount: Number(addWaterAmount) })
      });
      if (!res.ok) throw new Error('Add water failed');
      setToast({ message: 'Water added', type: 'success' });
      setAddWaterTargetId(null);
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  const handleConnect = (id) => {
    setConnectSourceId(id);
    setShowConnectModal(true);
  };

  const handleSelectConnectTarget = async (targetId) => {
    setShowConnectModal(false);
    setError('');
    try {
      const res = await fetch('/api/containers/connections', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ sourceContainerId: connectSourceId, targetContainerId: targetId })
      });
      if (!res.ok) throw new Error('Connect failed');
      setToast({ message: 'Connected!', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  return (
    <div className="container-app">
      <h1>Container Management</h1>
      <ContainerGraph containers={containers} handleConnect={handleConnect} handleDelete={handleDelete} handleAddWater={handleAddWater} />
      {addWaterTargetId && (
        <div style={{
          position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh',
          background: 'rgba(0,0,0,0.2)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 2000
        }}>
          <div style={{ background: '#fff', padding: 24, borderRadius: 8, minWidth: 300 }}>
            <h3>Add Water</h3>
            <input
              type="number"
              min={1}
              value={addWaterAmount}
              onChange={e => setAddWaterAmount(e.target.value)}
              style={{ width: 100, marginRight: 8 }}
            />
            <button onClick={handleConfirmAddWater} style={{ marginRight: 8 }}>Save</button>
            <button onClick={() => setAddWaterTargetId(null)}>Cancel</button>
          </div>
        </div>
      )}
      {showConnectModal && (
        <div style={{
          position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh',
          background: 'rgba(0,0,0,0.2)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 2000
        }}>
          <div style={{ background: '#fff', padding: 24, borderRadius: 8, minWidth: 300 }}>
            <h3>Choose Container</h3>
            <ul style={{ listStyle: 'none', padding: 0 }}>
              {containers
                .filter(c => c.id !== connectSourceId && !containers.find(src => src.id === connectSourceId)?.connectedContainerIds.includes(c.id))
                .map(c => (
                  <li key={c.id} style={{ margin: '8px 0' }}>
                    <button onClick={() => handleSelectConnectTarget(c.id)} style={{ width: '100%', padding: 8 }}>
                      {c.name}
                    </button>
                  </li>
                ))}
            </ul>
            <button onClick={() => setShowConnectModal(false)} style={{ marginTop: 12 }}>Cancel</button>
          </div>
        </div>
      )}
      {toast.message && (
        <div style={{
          position: 'fixed',
          top: 20,
          left: '50%',
          transform: 'translateX(-50%)',
          background: toast.type === 'success' ? '#4caf50' : '#f44336',
          color: '#fff',
          padding: '10px 24px',
          borderRadius: 6,
          zIndex: 1000,
          fontSize: 16,
          boxShadow: '0 2px 8px rgba(0,0,0,0.15)'
        }}>{toast.message}</div>
      )}
      {error && <div style={{ color: 'red' }}>{error}</div>}
      <div style={{ marginBottom: 16 }}>
        <input
          type="text"
          placeholder="Name"
          value={newContainerName}
          onChange={e => setNewContainerName(e.target.value)}
          style={{ width: 120, marginRight: 8 }}
        />
        <input
          type="number"
          min={1}
          value={newContainerCapacity}
          onChange={e => setNewContainerCapacity(Number(e.target.value))}
          style={{ width: 80 }}
        />
        <button onClick={handleAddContainer} style={{ marginLeft: 8 }}>Add Container</button>
      </div>
      {loading ? (
        <div>Loading...</div>
      ) : null}
    </div>
  );
}

export default App;
