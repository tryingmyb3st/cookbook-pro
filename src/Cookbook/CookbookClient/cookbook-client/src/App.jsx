import './App.css'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import MainPage from './components/MainPage/MainPage';
import Menu from './components/Menu/Menu'
import RecipePage from './components/RecipePage/RecipePage';
import MyRecipesPage from './components/MyRecipesPage/MyRecipesPage';
import ProtectedRoute from './components/ProtectedRoute';
import Register from './components/Auth/Register';
import Login from './components/Auth/Login';

function App() {
  return (
    <div className="App">
      <Router>
        <Menu />
        <Routes>
          <Route path='/' element={
            <ProtectedRoute>
              <MainPage />
            </ProtectedRoute>} />
          <Route path='/recipe/:id' element={
            <ProtectedRoute>
              <RecipePage />
            </ProtectedRoute>} />
          <Route path='/my' element={
            <ProtectedRoute>
              <MyRecipesPage />
            </ProtectedRoute>
          } />
          <Route path='/register' element={<Register />} />
          <Route path='/login' element={<Login />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App
