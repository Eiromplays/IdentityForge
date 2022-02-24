import { toast } from 'react-toastify';

export const NotFound = () => {
  toast.error('404, Page Not found!', { toastId: '404' });

  return (
    <div className="h-[100vh] flex items-center justify-center">
      <h1 style={{ fontSize: '40px' }}>404, PAGE NOT FOUND!</h1>
    </div>
  );
};
