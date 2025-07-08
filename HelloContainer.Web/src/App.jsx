import { useEffect, useState } from 'react';
import './App.css';

function ContainerGraph({ containers }) {
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

  return (
    <svg width={width} height={height} style={{ background: '#f8f8f8', borderRadius: 12, marginBottom: 32 }}>
      {containers.map((c, i) => (
        c.connectedContainerIds?.map(cid => {
          const from = getPos(c.id);
          const to = getPos(cid);
          if (!to || c.id > cid) return null;
          return (
            <line
              key={c.id + '-' + cid}
              x1={from.x + containerWidth}
              y1={from.y + containerHeight / 2}
              x2={to.x}
              y2={to.y + containerHeight / 2}
              stroke="#222"
              strokeWidth={2}
            />
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
  const [addAmount, setAddAmount] = useState(1);
  const [newContainerCapacity, setNewContainerCapacity] = useState(10);
  const [amountInputs, setAmountInputs] = useState({});
  const [toast, setToast] = useState({ message: '', type: '' }); // type: 'success' | 'error'
  const [newContainerName, setNewContainerName] = useState("");

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

  const handleAmountInputChange = (id, value) => {
    setAmountInputs(inputs => ({ ...inputs, [id]: value }));
  };

  const handleAddWater = async (id) => {
    setError('');
    const amount = Number(amountInputs[id]) || 1;
    try {
      const res = await fetch(`/api/containers/${id}/water`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount })
      });
      if (!res.ok) throw new Error('Add water failed');
      setToast({ message: 'Water added', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  return (
    <div className="container-app">
      <h1>Container Management</h1>
      <ContainerGraph containers={containers} />
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
      ) : (
        <table border="1" cellPadding="8" style={{ width: '100%', textAlign: 'center' }}>
          <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Capacity</th>
                <th>Amount</th>
                <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {containers.map(c => (
              <tr key={c.id}>
                <td>{c.id}</td>
                <td>{c.name}</td>
                <td>{c.capacity}</td>
                <td>{c.amount}</td>
                <td>
                  <input
                    type="number"
                    value={amountInputs[c.id] ?? 1}
                    min={1}
                    style={{ width: 60 }}
                    onChange={e => handleAmountInputChange(c.id, e.target.value)}
                  />
                  <button onClick={() => handleAddWater(c.id)} style={{ marginLeft: 4 }}>Add Water</button>
                  <button onClick={() => handleDelete(c.id)} style={{ marginLeft: 8, color: 'red' }}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;
