import { useEffect, useState } from 'react';
import './App.css';

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
