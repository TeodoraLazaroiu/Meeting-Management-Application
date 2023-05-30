import { Route, Routes } from 'react-router-dom';
import { Login } from './pages/login/Login'
import { Register } from './pages/register/Register'
import { ToastContainer } from 'react-toastify';
import { UserCalendar } from './pages/calendar/UserCalendar';

function App() {
  return (
    <div className="App">
      <ToastContainer/>
      <Routes>
        <Route path='/' element={<Login/>}/>
        <Route path='/login' element={<Login/>}/>
        <Route path='/register' element={<Register/>}/>
        <Route path='/calendar' element={<UserCalendar/>}/>
      </Routes>
    </div>
  );
}

export default App;
