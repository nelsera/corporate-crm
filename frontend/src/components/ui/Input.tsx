export function Input(props: React.InputHTMLAttributes<HTMLInputElement>) {
  return (
    <input
      {...props}
      className={[
        "h-11 w-full rounded-xl border border-slate-300 bg-white px-4 text-sm",
        "placeholder:text-slate-400",
        "focus:border-blue-500 focus:ring-2 focus:ring-blue-200",
        "transition-all duration-150",
        props.className ?? ""
      ].join(" ")}
    />
  );
}