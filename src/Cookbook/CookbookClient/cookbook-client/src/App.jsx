import './App.css'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import MainPage from './components/MainPage/MainPage';
import Menu from './components/Menu/Menu'
import RecipePage from './components/RecipePage/RecipePage';
import MyRecipesPage from './components/MyRecipesPage/MyRecipesPage';

function App() {
  return (
    <div className="App">
      <Router>
        <Menu />
        <Routes>
          <Route path='/' element={<MainPage />} />
          <Route path='/recipe/:id' element={<RecipePage />} />
          <Route path='/my' element={<MyRecipesPage />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App
