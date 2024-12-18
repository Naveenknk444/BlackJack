namespace BlackJackGameLogic
{
    public class Class1
    {


        <xsl:template match="Citation" mode="#all">
    <div class="pn-poms-citation">
        <span class="pn-poms-citation-title" style="font-weight: bold;">CITATIONS:</span>
        <xsl:text> </xsl:text>
        <span class="pn-poms-citation-text">
            <xsl:apply-templates/>
        </span>
    </div>
</xsl:template>







    }
}
