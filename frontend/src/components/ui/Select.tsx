export function Select(props: React.SelectHTMLAttributes<HTMLSelectElement>) {
  return (
    <select
      {...props}
      className={[
        "h-11 w-full rounded-xl border border-slate-300 bg-white px-4 text-sm",
        "focus:border-blue-500 focus:ring-2 focus:ring-blue-200",
        "transition-all duration-150",
        props.className ?? ""
      ].join(" ")}
    />
  );
}