namespace Loop.SGHSS.Domain.Defaults
{
    public class DefaultEntityIdModel
    {
        /// <summary>
        /// ID do registro.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Indica se foi deletado.
        /// </summary>
        public bool SysIsDeleted { get; protected set; }

        /// <summary>
        /// ID do usuário que inseriu o registro.
        /// </summary>
        public Guid SysUserInsert { get; protected set; }

        /// <summary>
        /// ID do usuário que atualizou o registro.
        /// </summary>
        public Guid SysUserUpdate { get; protected set; }

        /// <summary>
        /// Data em que o registro foi inserido.
        /// </summary>
        public DateTime SysDInsert { get; protected set; }

        /// <summary>
        /// Data em que o registro foi atualizado.
        /// </summary>
        public DateTime SysDUpdate { get; protected set; }

        public void Delete() => SysIsDeleted = true;
    }
}
