import { Route, Routes, useNavigate } from 'react-router-dom';
import { Login } from './pages/login/Login'
import { Register } from './pages/register/Register'
import { ToastContainer } from 'react-toastify';
import { Home } from './pages/calendar/Home';
import AuthContext from './utils/AuthContext';
import { useState, useEffect } from 'react';
import axios from 'axios';

function App() {
  const [authenticated, setAuthenticated] = useState(false);

  const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

  const navigate = useNavigate()
  useEffect(() => {
    const getUser = async () => {
    await client.get('/user')
        .then(() => {
            setAuthenticated(true)
        })
        .catch(() => {
            setAuthenticated(false)
            navigate('/login')
        })
      }
    getUser()
  }, []);

  return (
    <div className="App">
      <ToastContainer/>
      <AuthContext.Provider value={{ authenticated, setAuthenticated }}>
        <Routes>
          <Route path='/' element={<Login/>}/>
          <Route path='/login' element={<Login/>}/>
          <Route path='/register' element={<Register/>}/>
          <Route path='/home' element={<Home/>}/>
        </Routes>
      </AuthContext.Provider>
    </div>
  );
}

export default App;
