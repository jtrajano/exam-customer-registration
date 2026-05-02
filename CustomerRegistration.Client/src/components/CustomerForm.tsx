import React, { useState } from 'react';
import SignaturePad from './SignaturePad';

interface FormData {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  signatureBase64: string;
}

interface FormErrors {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  signatureBase64?: string;
}

const CustomerForm: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    signatureBase64: '',
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [submitError, setSubmitError] = useState<string | null>(null);

  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    if (!formData.firstName) newErrors.firstName = 'First name is required';
    if (!formData.lastName) newErrors.lastName = 'Last name is required';
    
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!formData.email) {
      newErrors.email = 'Email is required';
    } else if (!emailRegex.test(formData.email)) {
      newErrors.email = 'Invalid email format';
    }

    const phoneRegex = /^\+?[1-9]\d{1,14}$/;
    if (!formData.phoneNumber) {
      newErrors.phoneNumber = 'Phone number is required';
    } else if (!phoneRegex.test(formData.phoneNumber)) {
      newErrors.phoneNumber = 'Invalid phone number format (+1234567890)';
    }

    if (!formData.signatureBase64) {
      newErrors.signatureBase64 = 'Signature is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (errors[name as keyof FormErrors]) {
      setErrors((prev) => ({ ...prev, [name]: undefined }));
    }
  };

  const handleSignatureSave = (base64: string) => {
    setFormData(prev => ({ ...prev, signatureBase64: base64 }));
    if (errors.signatureBase64) {
      setErrors(prev => ({ ...prev, signatureBase64: undefined }));
    }
  };

  const handleSignatureClear = () => {
    setFormData(prev => ({ ...prev, signatureBase64: '' }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    setIsSubmitting(true);
    setSubmitError(null);

    try {
      const response = await fetch('/api/customers', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });

      if (response.ok) {
        setIsSuccess(true);
      } else {
        const errorData = await response.json();
        setSubmitError(errorData.detail || 'An error occurred during registration.');
      }
    } catch (err) {
      setSubmitError('Failed to connect to the server. Please ensure the API is running.');
    } finally {
      setIsSubmitting(false);
    }
  };

  if (isSuccess) {
    return (
      <div className="glass-card animate-fade-in" style={{ textAlign: 'center' }}>
        <div style={{ fontSize: '4rem', marginBottom: '1rem' }}>✅</div>
        <h2 style={{ color: 'var(--success)' }}>Registration Successful!</h2>
        <p style={{ marginBottom: '2rem' }}>
          Thank you, <strong>{formData.firstName}</strong>. Your onboarding profile has been created successfully.
        </p>
        <button className="primary-btn" onClick={() => window.location.reload()}>
          Register Another Customer
        </button>
      </div>
    );
  }

  return (
    <div className="glass-card animate-fade-in">
      <form onSubmit={handleSubmit} noValidate>
        {submitError && (
          <div className="error-message" style={{ 
            background: 'rgba(239, 68, 68, 0.1)', 
            padding: '1rem', 
            borderRadius: '8px', 
            marginBottom: '1.5rem',
            textAlign: 'center'
          }}>
            {submitError}
          </div>
        )}

        <div className="form-group">
          <label htmlFor="firstName">First Name</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            disabled={isSubmitting}
            className={errors.firstName ? 'error' : ''}
            placeholder="e.g. John"
          />
          {errors.firstName && <div className="error-message">{errors.firstName}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="lastName">Last Name</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            disabled={isSubmitting}
            className={errors.lastName ? 'error' : ''}
            placeholder="e.g. Doe"
          />
          {errors.lastName && <div className="error-message">{errors.lastName}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="email">Email Address</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            disabled={isSubmitting}
            className={errors.email ? 'error' : ''}
            placeholder="john.doe@example.com"
          />
          {errors.email && <div className="error-message">{errors.email}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="phoneNumber">Phone Number</label>
          <input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            disabled={isSubmitting}
            className={errors.phoneNumber ? 'error' : ''}
            placeholder="+1234567890"
          />
          {errors.phoneNumber && <div className="error-message">{errors.phoneNumber}</div>}
        </div>

        <div className="form-group">
          <SignaturePad 
            onSave={handleSignatureSave} 
            onClear={handleSignatureClear} 
          />
          {errors.signatureBase64 && <div className="error-message">{errors.signatureBase64}</div>}
        </div>

        <button type="submit" className="primary-btn" disabled={isSubmitting}>
          {isSubmitting ? 'Registering...' : 'Complete Registration'}
        </button>
      </form>
    </div>
  );
};


export default CustomerForm;
