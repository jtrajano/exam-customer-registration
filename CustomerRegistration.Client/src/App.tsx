import CustomerForm from './components/CustomerForm'
import './index.css'

function App() {
  return (
    <div className="app-container">
      <header className="animate-fade-in">
        <h1>Customer Onboarding</h1>
        <h2>Complete the registration form below to get started.</h2>
      </header>

      <main>
        <CustomerForm />
      </main>

      <footer style={{ marginTop: '4rem', color: 'var(--text-muted)', fontSize: '0.8rem' }}>
        <p>&copy; 2026 Customer Registration System. All rights reserved.</p>
      </footer>
    </div>
  )
}

export default App
