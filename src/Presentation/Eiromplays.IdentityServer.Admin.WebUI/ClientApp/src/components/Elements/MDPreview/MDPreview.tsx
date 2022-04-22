import createDOMPurify from 'dompurify';
import { marked } from 'marked';

const DOMPurify = createDOMPurify(window);

export type MDPreviewProps = {
  value: string;
  textColorClass?: string;
};

export const MDPreview = ({ value = '', textColorClass = '' }: MDPreviewProps) => {
  return (
    <div
      className={`p-2 w-full prose prose-indigo ${
        textColorClass ? textColorClass : 'dark:text-white'
      }`}
      dangerouslySetInnerHTML={{
        __html: DOMPurify.sanitize(marked(value)),
      }}
    />
  );
};
