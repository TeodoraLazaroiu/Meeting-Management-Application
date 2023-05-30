import { Route, Routes } from 'react-router-dom';
import { Login } from './pages/login/Login'
import { Register } from './pages/register/Register'
import { ToastContainer } from 'react-toastify';
import { Home } from './pages/calendar/Home';

function App() {
  return (
    <div className="App">
      <ToastContainer/>
      <Routes>
        <Route path='/' element={<Login/>}/>
        <Route path='/login' element={<Login/>}/>
        <Route path='/register' element={<Register/>}/>
        <Route path='/home' element={<Home/>}/>
      </Routes>
    </div>
  );
}

export default App;
