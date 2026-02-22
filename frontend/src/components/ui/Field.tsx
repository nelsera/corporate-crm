export function Field(props: {
  label: string;
  hint?: string;
  error?: string | null;
  children: React.ReactNode;
}) {
  return (
    <label className="flex flex-col gap-2">
      <span className="text-xs font-medium text-slate-600">
        {props.label}
      </span>

      {props.children}

      {props.error ? (
        <span className="text-xs text-red-500">{props.error}</span>
      ) : props.hint ? (
        <span className="text-xs text-slate-400">{props.hint}</span>
      ) : null}
    </label>
  );
}