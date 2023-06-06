import { Route, Routes, useNavigate } from 'react-router-dom';
import { Login } from './pages/Login'
import { Register } from './pages/Register'
import { ToastContainer } from 'react-toastify';
import { Home } from './pages/Home';
import AuthContext from './utils/AuthContext';
import { useState, useEffect } from 'react';
import axios from 'axios';
import { Team } from './pages/Team';
import { NoTeam } from './pages/NoTeam'

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
          <Route path='/team' element={<Team/>}/>
          <Route path='/join' element={<NoTeam/>}/>
        </Routes>
      </AuthContext.Provider>
    </div>
  );
}

export default App;
