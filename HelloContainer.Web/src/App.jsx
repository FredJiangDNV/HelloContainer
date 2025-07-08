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

  // 拉取容器列表
  const fetchContainers = async () => {
    setLoading(true);
    setError('');
    try {
      const res = await fetch('/api/containers');
      if (!res.ok) throw new Error('获取容器失败');
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

  // Toast 显示和自动消失
  useEffect(() => {
    if (toast.message) {
      const timer = setTimeout(() => setToast({ message: '', type: '' }), 2500);
      return () => clearTimeout(timer);
    }
  }, [toast]);

  // 添加容器
  const handleAddContainer = async () => {
    setError('');
    try {
      const res = await fetch('/api/containers', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: "A", capacity: newContainerCapacity })
      });
      if (!res.ok) throw new Error('添加容器失败');
      setToast({ message: '添加成功', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  // 删除容器
  const handleDelete = async (id) => {
    if (!window.confirm('确定要删除该容器吗？')) return;
    setError('');
    try {
      const res = await fetch(`/api/containers/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('删除容器失败');
      setToast({ message: '删除成功', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  // 加水/减水输入框变化
  const handleAmountInputChange = (id, value) => {
    setAmountInputs(inputs => ({ ...inputs, [id]: value }));
  };

  // 加水/减水
  const handleAddWater = async (id) => {
    setError('');
    const amount = Number(amountInputs[id]) || 1;
    try {
      const res = await fetch(`/api/containers/${id}/add-water`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount })
      });
      if (!res.ok) throw new Error('加水失败');
      setToast({ message: '加水成功', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  const handleRemoveWater = async (id) => {
    setError('');
    const amount = Number(amountInputs[id]) || 1;
    try {
      const res = await fetch(`/api/containers/${id}/remove-water`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount })
      });
      if (!res.ok) throw new Error('减水失败');
      setToast({ message: '减水成功', type: 'success' });
      await fetchContainers();
    } catch (e) {
      setError(e.message);
      setToast({ message: e.message, type: 'error' });
    }
  };

  return (
    <div className="container-app">
      <h1>容器管理</h1>
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
          type="number"
          min={1}
          value={newContainerCapacity}
          onChange={e => setNewContainerCapacity(Number(e.target.value))}
          style={{ width: 80 }}
        />
        <button onClick={handleAddContainer} style={{ marginLeft: 8 }}>添加容器</button>
      </div>
      {loading ? (
        <div>加载中...</div>
      ) : (
        <table border="1" cellPadding="8" style={{ width: '100%', textAlign: 'center' }}>
          <thead>
            <tr>
              <th>ID</th>
              <th>容量</th>
              <th>当前水量</th>
              <th>操作</th>
            </tr>
          </thead>
          <tbody>
            {containers.map(c => (
              <tr key={c.id}>
                <td>{c.id}</td>
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
                  <button onClick={() => handleAddWater(c.id)} style={{ marginLeft: 4 }}>加水</button>
                  <button onClick={() => handleRemoveWater(c.id)} style={{ marginLeft: 4 }}>减水</button>
                  <button onClick={() => handleDelete(c.id)} style={{ marginLeft: 8, color: 'red' }}>删除</button>
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
