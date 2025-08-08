import { useEffect, useState } from 'react';
import './App.css';
import ContainerGraph from './ContainerGraph';
import AddWaterModal from './AddWaterModal';
import ConnectModal from './ConnectModal';

function App() {
  const [containers, setContainers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [newContainerCapacity, setNewContainerCapacity] = useState(10);
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
      <AddWaterModal
        visible={!!addWaterTargetId}
        amount={addWaterAmount}
        onAmountChange={setAddWaterAmount}
        onConfirm={handleConfirmAddWater}
        onCancel={() => setAddWaterTargetId(null)}
      />
      <ConnectModal
        visible={showConnectModal}
        candidates={containers.filter(c => c.id !== connectSourceId && !containers.find(src => src.id === connectSourceId)?.connectedContainerIds.includes(c.id))}
        onSelect={handleSelectConnectTarget}
        onCancel={() => setShowConnectModal(false)}
      />
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
