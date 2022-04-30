import { Button } from '../Button';

type PageButtonProps = {
  page: number;

  isActive?: boolean;

  onClick: (page: number) => void;
};

export const PageButton = ({ page, isActive, onClick }: PageButtonProps) => {
  return (
    <Button size="sm" variant={isActive ? 'primary' : 'outline'} onClick={() => onClick(page)}>
      {page}
    </Button>
  );
};
